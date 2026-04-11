import tempfile
from fastapi import APIRouter, UploadFile, File

from src.core.application.skill_extraction_service import SkillExtractionService
from src.models.dtos.skill_dto import ExtractedSkillsResponse, SkillDto

router = APIRouter(prefix="/skills", tags=["skills"])

service = SkillExtractionService()


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