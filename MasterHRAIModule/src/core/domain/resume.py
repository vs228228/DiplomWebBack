from dataclasses import dataclass
from typing import Optional
from datetime import datetime


@dataclass
class Resume:
    raw_text: str
    source_path: Optional[str] = None
    candidate_id: Optional[str] = None
    created_at: datetime = datetime.utcnow()

    @property
    def is_empty(self) -> bool:
        return not self.raw_text or not self.raw_text.strip()

    def short_preview(self, length: int = 200) -> str:
        return self.raw_text[:length]
