class ProjectMatchingService:

    def __init__(self):
        from src.infrastructure.project_matcher import ProjectMatcher
        self.matcher = ProjectMatcher()

    def match(self, candidate_json, projects_json, mode: str = "all"):
        return self.matcher.match(candidate_json, projects_json, mode)