from dataclasses import dataclass
from typing import List
import os
from docx import Document
from PyPDF2 import PdfReader


@dataclass
class ExtractedDocument:
    text: str
    tables: List[List[List[str]]]


class TextExtractor:

    SUPPORTED_EXTENSIONS = (".txt", ".pdf", ".docx", ".doc")

    def extract(self, file_path: str) -> ExtractedDocument:

        if not os.path.exists(file_path):
            raise FileNotFoundError(file_path)

        ext = os.path.splitext(file_path)[1].lower()

        if ext == ".txt":
            return ExtractedDocument(
                text=self._extract_txt(file_path),
                tables=[]
            )

        if ext == ".pdf":
            return ExtractedDocument(
                text=self._extract_pdf(file_path),
                tables=[]
            )

        return self._extract_docx(file_path)

    def _extract_txt(self, path: str) -> str:
        with open(path, encoding="utf-8", errors="ignore") as f:
            return f.read()

    def _extract_pdf(self, path: str) -> str:
        reader = PdfReader(path)
        text = ""
        for p in reader.pages:
            t = p.extract_text()
            if t:
                text += t + "\n"
        return text

    def _extract_docx(self, path: str) -> ExtractedDocument:

        doc = Document(path)

        paragraphs = [p.text for p in doc.paragraphs if p.text.strip()]

        tables = []

        for table in doc.tables:
            t = []
            for row in table.rows:
                t.append([cell.text.strip() for cell in row.cells])
            tables.append(t)

        return ExtractedDocument(
            text="\n".join(paragraphs),
            tables=tables
        )