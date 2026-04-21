import tempfile
from fastapi import APIRouter, UploadFile, File

from src.core.application.skill_extraction_service import SkillExtractionService
from src.core.application.project_matching_service import ProjectMatchingService
from src.infrastructure.nlp.text_extractor import TextExtractor
from src.models.dtos.project_match_dto import MatchProjectsRequest
from src.models.dtos.skill_dto import ExtractedSkillsResponse, SkillDto
from pydantic import BaseModel
from src.infrastructure.nlp.ml_skill_extractor import MLSkillExtractor

router = APIRouter(prefix="/skills", tags=["skills"])

service = SkillExtractionService()
text_extractor = TextExtractor()
matching_service = ProjectMatchingService()

ml_extractor = MLSkillExtractor(model_path="ru_model")


@router.post("/extract", response_model=ExtractedSkillsResponse)
async def extract_skills(file: UploadFile = File(...)):

    contents = await file.read()

    with tempfile.NamedTemporaryFile(delete=False, suffix=file.filename) as tmp:
        tmp.write(contents)
        temp_path = tmp.name

    result = service.extract_from_file(temp_path)

    skills = [SkillDto(**s) for s in result.skills]

    return ExtractedSkillsResponse(
        skills=skills,
        total_found=result.total_found
    )

@router.post("/extract-pure", response_model=ExtractedSkillsResponse)
async def extract_skills(file: UploadFile = File(...)):

    contents = await file.read()

    with tempfile.NamedTemporaryFile(delete=False, suffix=file.filename) as tmp:
        tmp.write(contents)
        temp_path = tmp.name

    result = service.extract_from_file(temp_path, False)

    skills = [SkillDto(**s) for s in result.skills]

    return ExtractedSkillsResponse(
        skills=skills,
        total_found=result.total_found
    )

@router.post("/match-projects")
async def match_projects(request: MatchProjectsRequest, mode: str = "all"):

    candidate_dict = {
        "skills": [s.dict() for s in request.candidate.skills]
    }

    projects_dict = [
        {
            "id": p.id,
            "name": p.name,
            "skills": [s.dict() for s in p.skills]
        }
        for p in request.projects
    ]

    results = matching_service.match(
        candidate_json=candidate_dict,
        projects_json=projects_dict,
        mode=mode
    )

    return results

class TextRequest(BaseModel):
    text: str


@router.post("/extract-ml")
async def extract_skills_ml(
    file: UploadFile = File(None),
    request: TextRequest = None
):

    text = None

    if request and request.text:
        text = request.text

    elif file:

        contents = await file.read()

        suffix = file.filename.split(".")[-1] if file.filename else "tmp"

        with tempfile.NamedTemporaryFile(delete=False, suffix=f".{suffix}") as tmp:
            tmp.write(contents)
            temp_path = tmp.name

        doc = text_extractor.extract(temp_path)
        text = doc.text

    if not text:
        return {
            "error": "No input provided (file or text required)"
        }

    raw_skills = ml_extractor.extract(text)

    return {
        "skills": raw_skills,
        "total_found": len(raw_skills)
    }