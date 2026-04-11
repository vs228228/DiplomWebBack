import re
import spacy
from spacy.matcher import PhraseMatcher
from skillNer.skill_extractor_class import SkillExtractor
from skillNer.general_params import SKILL_DB


class SkillNerWrapper:

    def __init__(self):
        self.nlp = spacy.load("en_core_web_sm")

        self.extractor = SkillExtractor(
            self.nlp,
            SKILL_DB,
            phraseMatcher=PhraseMatcher
        )

    def extract(self, text: str):

        if not text or not text.strip():
            return []

        text = self._normalize_for_nlp(text)

        annotations = self.extractor.annotate(text)

        results = annotations.get("results", {})

        skills = []

        for match in results.get("full_matches", []):
            val = match.get("doc_node_value")
            if val:
                skills.append(val)

        for match in results.get("ngram_scored", []):
            val = match.get("doc_node_value")
            if val:
                skills.append(val)

        return list(set(skills))

    def _normalize_for_nlp(self, text: str) -> str:

        text = text.replace("\r", "\n")

        text = re.sub(r"\b(\w+)(\s+\1\b)+", r"\1", text, flags=re.IGNORECASE)

        replacements = {
            "ASP.NET Core": "ASPNETCORE_TOKEN",
            "ASP.NET": "ASPNET_TOKEN",
            ".NET": "DOTNET_TOKEN",
            "Node.js": "NODEJS_TOKEN",
            "C#": "CSHARP_TOKEN",
        }

        for k, v in replacements.items():
            text = text.replace(k, v)

        text = re.sub(r"\s*[,/|]\s*", ". ", text)

        text = re.sub(r"([a-z])\s+([A-Z])", r"\1. \2", text)

        text = re.sub(r"\.{2,}", ".", text)

        for k, v in replacements.items():
            text = text.replace(v, k)

        parts = [p.strip() for p in text.split(".") if p.strip()]

        seen = set()
        unique = []

        for p in parts:
            key = re.sub(r"\W+", "", p.lower())
            if key and key not in seen:
                seen.add(key)
                unique.append(p)

        return ".\n".join(unique) + "."