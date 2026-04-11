from dataclasses import dataclass
from src.infrastructure.nlp.text_extractor import TextExtractor
from src.infrastructure.nlp.skill_extractor import SkillExtractor


@dataclass
class ExtractedSkillsResult:
    skills: list
    total_found: int


class SkillExtractionService:

    def __init__(self):
        self.extractor = TextExtractor()
        self.skill_extractor = SkillExtractor()

    def extract_from_file(self, file_path: str) -> ExtractedSkillsResult:

        doc = self.extractor.extract(file_path)

        skill_profile = self.skill_extractor.extract(
            text=doc.text,
            tables=doc.tables
        )

        skills_data = [
            {
                "name": s.name,
                "level": s.level,
                "years": s.years,
            }
            for s in skill_profile.skills
        ]

        return ExtractedSkillsResult(
            skills=skills_data,
            total_found=len(skills_data),
        )