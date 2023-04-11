import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css']
})
export class FormComponent implements OnInit {
  model: any;

  constructor() { }

  ngOnInit(): void {
  }

  register() {
    console.log(this.model);
  }

  cancel() {
    console.log("cancelled")
  }

}
