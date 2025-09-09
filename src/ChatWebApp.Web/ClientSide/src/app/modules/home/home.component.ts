import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public title: string;
  public linkSwagger: string;
  public username: string;
  public password: string;
  public currentYear: number;

  constructor() {
    this.linkSwagger = `http://localhost:5017/swagger/index.html`;
    this.title = "Chat WebApp";
    this.username = "admin@example.com";
    this.password = "admin";
    this.currentYear = new Date().getFullYear();
  }

  ngOnInit(): void {
  }

}
