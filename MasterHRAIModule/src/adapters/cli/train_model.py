import argparse
from src.infrastructure.ml.train_model import train


def main():
    parser = argparse.ArgumentParser()

    parser.add_argument("--data", required=True)
    parser.add_argument("--out", default="ru_model")

    args = parser.parse_args()

    train(args.data, args.out)


if __name__ == "__main__":
    main()