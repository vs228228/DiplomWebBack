from dataclasses import dataclass
from typing import List
from src.core.domain.skill import Skill


@dataclass
class SkillProfile:
    skills: List[Skill]

    def total_experience(self) -> float:
        return sum(skill.years for skill in self.skills)

    def filter_by_category(self, category: str):
        return [s for s in self.skills if s.category == category]
