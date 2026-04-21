from src.infrastructure.nlp.llm_skill_labeler import LLMSkillLabeler


class LLMOnlySkillExtractor:
    def __init__(self):
        self.llm = LLMSkillLabeler()

    def extract(self, text):
        return self.llm.extract(text)