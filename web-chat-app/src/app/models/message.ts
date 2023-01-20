import { SentimentModel } from "./sentiment";

export class MessageDto extends SentimentModel {
  public user: string = '';
  public msgText: string = '';
  public dateTime: string = '';
}
