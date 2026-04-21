from pydantic import BaseModel
from typing import List, Optional


class SkillInputDto(BaseModel):
    name: str
    level: Optional[str] = None
    years: Optional[float] = None


class CandidateDto(BaseModel):
    skills: List[SkillInputDto]


class ProjectDto(BaseModel):
    id: int
    name: str
    skills: List[SkillInputDto]


class MatchProjectsRequest(BaseModel):
    candidate: CandidateDto
    projects: List[ProjectDto]