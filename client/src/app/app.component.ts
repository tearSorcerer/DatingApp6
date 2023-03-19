import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Dating App';
  users: any;

  // localhost:
  // -- https://localhost:4200/
  // Directories:
  // - client:    cd C:\projects\NielCummings\DatingApp\client
  //    ng serve
  // - API:       cd C:\projects\NielCummings\DatingApp\api
  //    dotnet run

  // ngx link: https://valor-software.com/ngx-bootstrap/#/documentation#getting-started

  // Migrations
  // -- \API>dotnet ef migrations add UserPasswordAdded
  // -- \API>dotnet ef database update

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response=> this.users = response,
      error: error => console.log(error),
      complete: () => console.log('Request has completed')
    })
  }
}
