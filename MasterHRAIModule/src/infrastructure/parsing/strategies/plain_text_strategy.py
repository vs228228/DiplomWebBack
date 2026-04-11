import re
from typing import List
from src.core.domain.skill import Skill
from base_strategy import SkillParsingStrategy


class PlainTextStrategy(SkillParsingStrategy):

    SKILL_PATTERN = re.compile(
        r"\b(c#|\.net|asp\.net|ef core|angular|docker|redis|rabbitmq|postgresql|mssql|mongodb|git|swagger|postman)\b",
        re.IGNORECASE
    )

    def parse(self, text: str) -> List[Skill]:

        skills = []

        for match in self.SKILL_PATTERN.finditer(text):

            skills.append(
                Skill(
                    name=match.group(0),
                    category="text",
                    years=0,
                    level=None,
                    source_section="text",
                    last_used=None,
                )
            )

        return skills