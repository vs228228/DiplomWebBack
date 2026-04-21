from transformers import AutoTokenizer, AutoModelForTokenClassification, pipeline
from dataclasses import dataclass
from typing import List, Dict
import re


@dataclass
class MLSkill:
    name: str
    category: str = "ml_model"


class MLSkillExtractor:

    def __init__(self, model_path: str = "ru_model"):
        self.tokenizer = AutoTokenizer.from_pretrained(
            model_path,
            use_fast=True
        )

        self.model = AutoModelForTokenClassification.from_pretrained(model_path)

        self.ner = pipeline(
            "token-classification",
            model=self.model,
            tokenizer=self.tokenizer,
            aggregation_strategy="simple"
        )

    def extract(self, text: str) -> List[Dict]:
        if not text:
            return []

        chunks = split_text(text, max_words=200)

        results = []
        for chunk in chunks:
            results.extend(self.ner(chunk))

        skills = []

        for r in results:
            raw_entity = r.get("word", "")

            parts = split_combined_skills(raw_entity)

            for part in parts:
                entity = normalize_skill(part)

                if not entity or not is_valid_skill(entity):
                    continue

                skills.append({
                    "name": entity,
                    "category": r.get("entity_group", "unknown")
                })

        unique = {}
        for s in skills:
            key = s["name"].lower()
            if key not in unique:
                unique[key] = s

        final_skills = list(unique.values())

        return final_skills


def split_text(text: str, max_words: int = 200) -> List[str]:
    words = text.split()
    chunks = []

    for i in range(0, len(words), max_words):
        chunk = " ".join(words[i:i + max_words])
        if chunk.strip():
            chunks.append(chunk)

    return chunks


def normalize_skill(text: str) -> str:
    text = text.strip()

    text = re.sub(r"\s*([#\.+\-])\s*", r"\1", text)

    lowered = text.lower()

    replacements = {
        "C#": ["c #", "c  #"],
        "ASP.NET Core": ["asp.net core", "asp. net core", "asp .net core"],
        "Node.js": ["nodejs", "node js"],
        "ABP.io": ["abp. io", "abp io"],
        "Entity Framework Core": ["ef core", "ef"],
        "JavaScript": ["javascript"],
        "TypeScript": ["typescript"],
        "Fluent Assertions": ["fluent assertions"],
        "Identity Server": ["identity server"],
        "MediatR": ["mediat", "mediatr"],
        "Domain-Driven-Design": ["domain driven", "ddd", "Domain-driven"],
    }

    for correct, variants in replacements.items():
        if lowered == correct.lower() or lowered in variants:
            return correct

    text = text.strip(". ")

    if text.islower():
        text = text.capitalize()

    return text


def is_valid_skill(name: str) -> bool:
    if not name:
        return False

    name = name.strip()

    if len(name) < 2:
        return False

    blacklist = {
        "core", "unit", "server", "domain",
        "net", "asp", "abp"
    }

    if name.lower() in blacklist:
        return False

    # обрезки типа Mediat
    if len(name) < 5 and name.lower() not in {"c#", "grpc"}:
        return False

    # опыт
    if re.search(r"\d+\s*(год|года|лет|years?)", name.lower()):
        return False

    return True


def split_combined_skills(text: str) -> List[str]:
    text = text.strip()

    if re.search(r"[,\|/;]", text):
        parts = re.split(r"[,\|/;]+", text)
        return [p.strip() for p in parts if p.strip()]

    return [text]

def merge_known_patterns(skills: List[Dict]) -> List[Dict]:
    names = [s["name"] for s in skills]

    merged = []
    skip = set()

    def has(i, val):
        return i < len(names) and names[i].lower() == val.lower()

    i = 0
    while i < len(names):
        if i in skip:
            i += 1
            continue

        current = names[i]

        if has(i, "ASP.") and has(i+1, "Core"):
            merged.append({"name": "ASP.NET Core", "category": skills[i]["category"]})
            skip.update({i, i+1})
            i += 2
            continue

        if has(i, "Fluent") and has(i+1, "Assertions"):
            merged.append({"name": "Fluent Assertions", "category": skills[i]["category"]})
            skip.update({i, i+1})
            i += 2
            continue

        if has(i, "JetBrains") and has(i+1, "Rider"):
            merged.append({"name": "JetBrains Rider", "category": skills[i]["category"]})
            skip.update({i, i+1})
            i += 2
            continue

        if has(i, "Identity") and has(i+1, "Server"):
            merged.append({"name": "Identity Server", "category": skills[i]["category"]})
            skip.update({i, i+1})
            i += 2
            continue

        if has(i, "Domain"):
            merged.append({"name": "Domain-driven design", "category": skills[i]["category"]})
            skip.add(i)
            i += 1
            continue

        merged.append(skills[i])
        i += 1

    return merged