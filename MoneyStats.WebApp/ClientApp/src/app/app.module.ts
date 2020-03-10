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
import { FileFileComparerComponent } from './components/file-file-comparer/file-file-comparer.component';
import { DbFileComparerComponent } from './components/db-file-comparer/db-file-comparer.component';

// Pages
import { HomePage } from './pages/home-page/home.page';
import { TestTablePage } from './pages/test-table-page/test-table.page';
import { ExcelTestPage } from './pages/excel-test-page/excel-test.page';
import { UpdatePage } from './pages/update-page/update.page';

// Services
import { LoadingScreenService } from './services/loading-screen-service/loading-screen.service';
import { RouterService } from './services/router-service/router.service';
import { TestTableService } from './services/test-table-service/test-table.service';
import { BaseHttpService } from './services/base-http.service';
import { TransactionService } from './services/transaction-service/transaction.service';

@NgModule({
  declarations: [
    // Components
    AppComponent,
    NavComponent,
    LoadingScreenComponent,
    FileFileComparerComponent,
    DbFileComparerComponent,
    // Pages
    HomePage,
    TestTablePage,
    ExcelTestPage,
    UpdatePage
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FontAwesomeModule,
    RouterModule.forRoot([
      { path: '', component: HomePage },
      { path: 'testtable', component: TestTablePage },
      { path: 'update', component: UpdatePage },
      { path: 'excel', component: ExcelTestPage },
    ], { useHash: true })
  ],
  providers: [
    LoadingScreenService,
    RouterService,
    BaseHttpService,
    TestTableService,
    TransactionService
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
