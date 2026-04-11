from langdetect import detect, LangDetectException


class LanguageDetector:

    def detect(self, text: str) -> str:
        try:
            lang = detect(text)
        except LangDetectException:
            return "en"

        if lang.startswith("ru"):
            return "ru"

        return "en"