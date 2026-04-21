import argparse
from src.infrastructure.ml.dataset_builder import DatasetBuilder
from dotenv import load_dotenv


def main():
    load_dotenv()

    parser = argparse.ArgumentParser()

    parser.add_argument("--input", required=True)
    parser.add_argument("--output", default="dataset.jsonl")

    args = parser.parse_args()

    builder = DatasetBuilder()
    builder.build_from_folder(args.input, args.output)


if __name__ == "__main__":
    main()