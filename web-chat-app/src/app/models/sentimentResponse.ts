import { SentenceModel } from "./sentence";
import { SentimentModel } from "./sentiment";

export class SentimentResponseModel {
    public documentSentiment?: SentimentModel;
    public language?: string;
    public sentences?: [SentenceModel];
  }