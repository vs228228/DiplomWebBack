from abc import ABC, abstractmethod
from typing import List
from src.core.domain.skill import Skill


class SkillParsingStrategy(ABC):

    @abstractmethod
    def parse(self, text: str) -> List[Skill]:
        pass