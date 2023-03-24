import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Dating App';

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

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    this.setCurrentUser();
  }



  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return;

    const user: User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);

  }
}
