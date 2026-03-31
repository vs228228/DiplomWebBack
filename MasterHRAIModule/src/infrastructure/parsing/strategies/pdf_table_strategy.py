import re
from typing import List
from src.core.domain.skill import Skill
from base_strategy import SkillParsingStrategy


class PdfTableStrategy(SkillParsingStrategy):

    PATTERN = re.compile(
        r"""
        ^\s*
        (?P<name>[A-Za-zА-Яа-я0-9#\+\.\-\/ ]+?)
        \s+
        (?P<level>Beginner|Intermediate|Advanced|Базовый|Средний|Продвинутый)
        \s+
        (?P<years>\d+(\.\d+)?)
        \s+
        (?P<last_used>\d{4})
        """,
        re.IGNORECASE | re.MULTILINE | re.VERBOSE
    )

    def parse(self, text: str) -> List[Skill]:

        skills = []

        for match in self.PATTERN.finditer(text):

            skills.append(
                Skill(
                    name=match.group("name").strip(),
                    category="table",
                    years=float(match.group("years")),
                    level=match.group("level"),
                    source_section="technical_skills",
                    last_used=int(match.group("last_used")),
                )
            )

        return skills