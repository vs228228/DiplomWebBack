from pydantic import BaseModel


class SkillDto(BaseModel):
    name: str
    level: str | None = None
    years: float | None = None


class ExtractedSkillsResponse(BaseModel):
    skills: list[SkillDto]
    total_found: int