import json
import re
import numpy as np
from datasets import Dataset
from transformers import (
    AutoTokenizer,
    AutoModelForTokenClassification,
    TrainingArguments,
    Trainer,
    DataCollatorForTokenClassification
)
from seqeval.metrics import f1_score, precision_score, recall_score

MODEL_NAME = "bert-base-multilingual-cased"

LABELS = [
    "O",
    "B-HARD_SKILL", "I-HARD_SKILL",
    "B-APPLIED_SKILL", "I-APPLIED_SKILL",
    "B-EXPERIENCE", "I-EXPERIENCE"
]

LABEL2ID = {l: i for i, l in enumerate(LABELS)}
ID2LABEL = {i: l for l, i in LABEL2ID.items()}


def load_dataset(path: str):
    data = []
    with open(path, encoding="utf-8") as f:
        for line in f:
            data.append(json.loads(line))
    return Dataset.from_list(data)


def find_spans(text: str, skills: list):
    spans = []
    text_lower = text.lower()

    for skill in skills:
        name = skill["name"].lower()
        name_esc = re.escape(name)

        pattern = rf'(?<![a-zA-Z0-9]){name_esc}(?![a-zA-Z0-9])'

        for match in re.finditer(pattern, text_lower):
            skill_span = {
                "start": match.start(),
                "end": match.end(),
                "label": skill["category"]
            }
            spans.append(skill_span)

            context_start = skill_span["end"]
            context_end = min(len(text_lower), context_start + 60)
            context_text = text_lower[context_start:context_end]

            exp_pattern = r'(\d+([.,]\d+)?)\s*(лет|года|год|мес|years|yrs|yr)'
            exp_match = re.search(exp_pattern, context_text)

            if exp_match:
                spans.append({
                    "start": context_start + exp_match.start(),
                    "end": context_start + exp_match.end(),
                    "label": "EXPERIENCE"
                })

    spans = sorted(spans, key=lambda x: x["start"])
    unique_spans = []
    last_end = -1
    for span in spans:
        if span["start"] >= last_end:
            unique_spans.append(span)
            last_end = span["end"]
    return unique_spans


def tokenize_and_align(examples, tokenizer):
    tokenized_inputs = tokenizer(
        examples["text"],
        truncation=True,
        padding="max_length",
        max_length=512,
        stride=50,
        return_overflowing_tokens=True,
        return_offsets_mapping=True
    )

    sample_map = tokenized_inputs.pop("overflow_to_sample_mapping")
    offset_mapping = tokenized_inputs.pop("offset_mapping")
    labels = []

    for i, offsets in enumerate(offset_mapping):
        sample_idx = sample_map[i]
        text = examples["text"][sample_idx]
        skills = examples["skills"][sample_idx]

        current_spans = find_spans(text, skills)

        token_labels = [LABEL2ID["O"]] * len(tokenized_inputs["input_ids"][i])

        for span in current_spans:
            start, end, label = span["start"], span["end"], span["label"]
            found_b = False

            for idx, (s, e) in enumerate(offsets):
                if s == 0 and e == 0:
                    token_labels[idx] = -100
                    continue

                if e > start and s < end:
                    if not found_b:
                        token_labels[idx] = LABEL2ID[f"B-{label}"]
                        found_b = True
                    else:
                        token_labels[idx] = LABEL2ID[f"I-{label}"]

        labels.append(token_labels)

    tokenized_inputs["labels"] = labels
    return tokenized_inputs


def compute_metrics(eval_pred):
    logits, labels = eval_pred
    preds = np.argmax(logits, axis=2)

    true_preds = []
    true_labels = []

    for p, l in zip(preds, labels):
        p_list, l_list = [], []
        for pi, li in zip(p, l):
            if li == -100: continue
            p_list.append(LABELS[pi])
            l_list.append(LABELS[li])
        true_preds.append(p_list)
        true_labels.append(l_list)

    return {
        "precision": precision_score(true_labels, true_preds),
        "recall": recall_score(true_labels, true_preds),
        "f1": f1_score(true_labels, true_preds),
    }


def train(dataset_path: str, output_dir: str):
    raw_dataset = load_dataset(dataset_path)
    tokenizer = AutoTokenizer.from_pretrained(MODEL_NAME, use_fast=True)

    dataset = raw_dataset.map(
        lambda x: tokenize_and_align(x, tokenizer),
        batched=True,
        remove_columns=raw_dataset.column_names
    )

    dataset = dataset.train_test_split(test_size=0.1)

    model = AutoModelForTokenClassification.from_pretrained(
        MODEL_NAME,
        num_labels=len(LABELS),
        id2label=ID2LABEL,
        label2id=LABEL2ID
    )

    training_args = TrainingArguments(
        output_dir=output_dir,
        learning_rate=3e-5,
        per_device_train_batch_size=12,
        per_device_eval_batch_size=12,
        num_train_epochs=5,
        weight_decay=0.01,
        eval_strategy="epoch",
        save_strategy="epoch",
        load_best_model_at_end=True,
        metric_for_best_model="f1",
        report_to="none"
    )

    trainer = Trainer(
        model=model,
        args=training_args,
        train_dataset=dataset["train"],
        eval_dataset=dataset["test"],
        data_collator=DataCollatorForTokenClassification(tokenizer),
        compute_metrics=compute_metrics,
    )

    trainer.train()
    model.save_pretrained(output_dir)
    tokenizer.save_pretrained(output_dir)


if __name__ == "__main__":
    train("dataset.jsonl", "./skill_model_v1")