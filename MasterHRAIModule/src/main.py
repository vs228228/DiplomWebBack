from fastapi import FastAPI
from src.controllers.skill_controller import router as skill_router

app = FastAPI(
    title="Resume Skill Extraction Service",
    version="1.0"
)

app.include_router(skill_router)