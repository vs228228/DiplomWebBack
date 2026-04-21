import os
import json
import re
import time
import unicodedata
from typing import List


from src.infrastructure.nlp.llm_skill_labeler import LLMSkillLabeler
from src.infrastructure.nlp.text_extractor import TextExtractor
from src.infrastructure.ml.dataset_filter import DatasetFilter

class DatasetBuilder:
    def __init__(self):
        self.extractor = TextExtractor()
        self.llm_extractor = LLMSkillLabeler()
        self.filter = DatasetFilter()

    def build_from_folder(self, folder_path: str, output_path: str):
        dataset = []
        files = self._collect_files(folder_path)

        for file_path in files:
            try:
                doc = self.extractor.extract(file_path)

                clean_text = self._prepare_text_for_ner(doc.text)

                hard_skills, applied_skills = self.llm_extractor.extract(clean_text)

                time.sleep(2)

                skills_for_sample = []

                for s in hard_skills:
                    name = self._soft_normalize(s)
                    if name and self.filter.is_valid_skill(name):
                        skills_for_sample.append({
                            "name": name,
                            "category": "HARD_SKILL",
                            "years": None
                        })

                for s in applied_skills:
                    name = s.strip().lower()
                    if len(name) > 5:
                        skills_for_sample.append({
                            "name": name,
                            "category": "APPLIED_SKILL",
                            "years": None
                        })

                if skills_for_sample:
                    sample = {
                        "text": clean_text,
                        "skills": skills_for_sample
                    }
                    dataset.append(sample)
                    print(f"[OK] {file_path} (Найдено навыков: {len(skills_for_sample)})")

            except Exception as e:
                print(f"[ERROR] {file_path}: {e}")

        self._save(dataset, output_path)

    def _soft_normalize(self, text: str) -> str:
        if not text: return ""
        text = unicodedata.normalize("NFC", text)
        return text.strip()

    def _prepare_text_for_ner(self, text: str) -> str:
        text = re.sub(r'\n\s*\n', '\n', text)
        text = "".join(ch for ch in text if unicodedata.category(ch)[0] != "C" or ch == '\n')
        return text.strip()

    def _collect_files(self, folder: str) -> List[str]:
        result = []
        for root, _, files in os.walk(folder):
            for f in files:
                if f.lower().endswith((".txt", ".pdf", ".docx")):
                    result.append(os.path.join(root, f))
        return result

    def _save(self, dataset, path):
        with open(path, "w", encoding="utf-8") as f:
            for item in dataset:
                f.write(json.dumps(item, ensure_ascii=False) + "\n")
        print(f"Dataset saved to {path}. Total: {len(dataset)}")