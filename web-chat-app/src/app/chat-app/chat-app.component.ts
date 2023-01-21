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

    let username = localStorage.getItem('username');
    this.form = this.formBuilder.group({
      username: [username, Validators.required],
      msgText: ['', Validators.required],
    });
  }

  msgInboxArray: MessageDto[] = [];

  get f() {
    return this.form.controls;
  }
  
  onSubmit() {
    if (this.form.invalid) {
      window.alert('Both fields are required.');
      return;
    }
   
    this.sendMessage();
  }
  
  sendMessage(){
    let msgDto = new MessageDto();
    msgDto.user = this.f.username.value;
    msgDto.msgText = this.f.msgText.value;
    this.chatService.broadcastMessage(msgDto);

    this.form.controls['username'].disable();
    this.f.msgText.setValue('');
    window.setInterval(function () {
      var elem = document.getElementById('body');
      elem!.scrollTop = elem!.scrollHeight;
    }, 5000);
  }

  addToInbox(obj: MessageDto) {
    let newObj = new MessageDto();
    newObj.user = obj.user;
    newObj.msgText = obj.msgText;
    newObj.dateTime = new Date().toLocaleString();
    this.msgInboxArray.push(newObj);
  }
  counter(i: number) {
    return new Array(i);
  }
}
