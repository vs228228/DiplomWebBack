import re
from typing import List
from src.core.domain.skill import Skill
from base_strategy import SkillParsingStrategy


class DocxTableStrategy(SkillParsingStrategy):

    PATTERN = re.compile(
        r"""
        ^\s*
        (?P<name>.+?)
        \s*\|\s*
        (?P<level>Beginner|Intermediate|Advanced)
        \s*\|\s*
        (?P<years>\d+(\.\d+)?)
        \s*\|\s*
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