from dataclasses import dataclass
from src.infrastructure.nlp.skill_extractor_ml_only import SkillExtractor


@dataclass
class ExtractedSkillsResult:
    skills: list
    total_found: int


class SkillExtractionService:

    def __init__(self, model_path: str):
        self.extractor = SkillExtractor(model_path)

    def extract_from_file(self, file_path: str):

        from src.infrastructure.nlp.text_extractor import TextExtractor

        doc = TextExtractor().extract(file_path)

        profile = self.extractor.extract(doc.text, doc.tables)

        skills_data = [
            {
                "name": s.name,
                "category": s.category,
            }
            for s in profile.skills
        ]

        return ExtractedSkillsResult(
            skills=skills_data,
            total_found=len(skills_data)
        )