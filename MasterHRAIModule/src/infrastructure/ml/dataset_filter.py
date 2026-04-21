import re

class DatasetFilter:

    STOP_WORDS = [
        "что", "позволяет", "процесс", "ресурсы",
        "работы", "командной", "успешно"
    ]

    def is_valid_skill(self, text: str) -> bool:
        text = text.lower().strip()

        if len(text) < 2 or len(text) > 30:
            return False

        # максимум 2 слова
        if len(text.split()) > 2:
            return False

        bad_patterns = [
            "что", "котор", "процесс", "ресурс",
            "обеспеч", "позвол", "разработк",
            "проектирован", "анализ"
        ]

        if any(p in text for p in bad_patterns):
            return False

        return True

    def _looks_like_skill(self, skill: str):

        if re.search(r"[a-zA-Z]", skill):
            return True

        if re.search(r"[+#\.]", skill):
            return True

        if re.match(
            r"(анализ|разработка|проектирование|тестирование|оптимизация)",
            skill
        ):
            return True

        return False