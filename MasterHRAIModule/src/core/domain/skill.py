from dataclasses import dataclass
from typing import Optional


@dataclass
class Skill:
    name: str
    category: str
    years: Optional[float] = None
    level: Optional[str] = None
    source_section: Optional[str] = None
    last_used: Optional[int] = None
