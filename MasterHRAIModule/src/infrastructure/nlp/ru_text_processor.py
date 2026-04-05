from natasha import Segmenter, MorphVocab, NewsEmbedding, NewsMorphTagger, Doc


class RussianTextProcessor:

    def __init__(self):
        self.segmenter = Segmenter()
        self.morph_vocab = MorphVocab()
        self.emb = NewsEmbedding()
        self.morph_tagger = NewsMorphTagger(self.emb)

    def process(self, text: str):

        doc = Doc(text)

        doc.segment(self.segmenter)
        doc.tag_morph(self.morph_tagger)

        tokens = []

        for token in doc.tokens:
            token.lemmatize(self.morph_vocab)
            tokens.append(token.lemma.lower())

        return tokens