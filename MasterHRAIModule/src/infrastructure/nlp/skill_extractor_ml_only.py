from src.infrastructure.nlp.ml_skill_extractor import MLSkillExtractor
from src.core.domain.skill import Skill
from src.core.domain.skill_profile import SkillProfile


class SkillExtractor:

    def __init__(self, model_path: str):
        self.ml_extractor = MLSkillExtractor(model_path)

    def extract(self, text: str, tables: list = None) -> SkillProfile:

        ml_skills = self.ml_extractor.extract(text)

        skills = [
            Skill(
                name=s,
                category="ml_model",
                source_section="text"
            )
            for s in ml_skills
        ]

        return SkillProfile(skills=skills)