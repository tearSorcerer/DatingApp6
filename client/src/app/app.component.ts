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

  // --dry-run

  // Migrations
  // -- \API>dotnet ef migrations add UserPasswordAdded
  // -- \API>dotnet ef database update

  // NOTE TO FUTURE SELF: 
  // - for some reason the website doesnt work on edge...
  // might have to do with localStorage that has user auth info on it. 

  // Fun fact:
  // if you use take(1), after the subscription has taken, it will unsubscribe 

  // for that answer you provided: 'ɵɵComponentDeclaration'

  // DOTNET VERSIONS: https://dotnet.microsoft.com/en-us/download/visual-studio-sdks

  //  Upgrade dotnet versions:
  // https://learn.microsoft.com/en-us/aspnet/core/migration/60-70?view=aspnetcore-7.0&tabs=visual-studio
  // some useful commands:
  //  - dotnet --list-sdks
  //  - dotnet --version

  // Filter out:
  // -typeerror
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
