import { Component, OnInit } from '@angular/core';
import { ChatService } from 'src/app/services/chat.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
  faPaperPlane
} from '@fortawesome/free-solid-svg-icons';

import { environment } from 'src/environments/environment';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MessageDto } from '../models/message';

@Component({
  selector: 'app-chat-app',
  templateUrl: './chat-app.component.html',
  styleUrls: ['./chat-app.component.scss'],
})
export class ChatAppComponent implements OnInit {
  faPaperPlane = faPaperPlane;
  form!: FormGroup;
  overallScore?: number;

  constructor(
    private chatService: ChatService,
    private formBuilder: FormBuilder,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.chatService
      .retrieveMappedObject()
      .subscribe((receivedObj: MessageDto) => {
        this.addToInbox(receivedObj);
      }); // calls the service method to get the new messages sent

    this.form = this.formBuilder.group({
      username: ['', Validators.required],
      msgText: ['', Validators.required],
    });
  }

  msgInboxArray: MessageDto[] = [];

  get f() {
    return this.form.controls;
  }

  onSubmit() {
    //Check if username and message is not empty
    if (this.form.invalid) {
      window.alert('Both fields are required.');
      return;
    }

    //If valid, send message
    this.sendMessage();
  }

  sendMessage(){
    //Create message
    let msgDto = new MessageDto();
    msgDto.user = this.f.username.value;
    msgDto.msgText = this.f.msgText.value;

    //Send message, implementation of broadcastMessage you can see under services/chat.service.ts
    this.chatService.broadcastMessage(msgDto);

    //Clean text message
    this.f.msgText.setValue('');

    //Scroll whole chat panel so you'll see latest message
    window.setInterval(function () {
      var elem = document.getElementById('body');
      elem!.scrollTop = elem!.scrollHeight;
    }, 5000);
  }

  //Add this message to msgInboxArray
  addToInbox(obj: MessageDto) {
    let newObj = new MessageDto();
    newObj.user = obj.user;
    newObj.msgText = obj.msgText;
    newObj.dateTime = new Date().toLocaleString();
    this.msgInboxArray.push(newObj);
  }
}
