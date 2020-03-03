import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { HttpClientModule } from '@angular/common/http';
import { faInfoCircle, faCaretRight, faCaretLeft, faSun, faCog, faTimes, faTag, faAlignJustify } from '@fortawesome/free-solid-svg-icons';

// Components
import { AppComponent } from './app.component';
import { NavComponent } from './components/nav/nav.component';
import { LoadingScreenComponent } from './components/loading-screen/loading-screen.component';

// Pages
import { HomePage } from './pages/home-page/home.page';
import { TestTablePage } from './pages/test-table-page/test-table.page';

// Services
import { LoadingScreenService } from './services/loading-screen-service/loading-screen.service';
import { RouterService } from './services/router-service/router.service';
import { TestTableService } from './services/test-table-service/test-table.service';
import { BaseHttpService } from './services/base-http.service';

@NgModule({
  declarations: [
    // Components
    AppComponent,
    NavComponent,
    LoadingScreenComponent,
    // Pages
    HomePage,
    TestTablePage
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FontAwesomeModule,
    RouterModule.forRoot([
      { path: '', component: HomePage },
      { path: 'testtable', component: TestTablePage },
    ], { useHash: true })
  ],
  providers: [
    LoadingScreenService,
    RouterService,
    BaseHttpService,
    TestTableService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
  constructor() {
      library.add(faInfoCircle);
      library.add(faCog);
      library.add(faAlignJustify);
      library.add(faTag);
  }
}
