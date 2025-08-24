import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { TarefaService } from '../../../services/tarefa.service';
import { catchError } from 'rxjs';
import { ITarefa } from '../../../interfaces/ITarefa';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { FormTarefasComponent } from '../form-tarefas/form-tarefas.component';
import { ModalConfirmarExclusaoComponent } from '../modal-confirmar-exclusao/modal-confirmar-exclusao.component';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-tarefas',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatPaginatorModule,
    MatSortModule,
    FormTarefasComponent,
    ModalConfirmarExclusaoComponent
  ],
  templateUrl: './list-tarefas.component.html',
  styleUrl: './list-tarefas.component.css'
})
export class ListTarefasComponent implements OnInit, AfterViewInit {

  tarefas: ITarefa[] = [];
  dataSource!: MatTableDataSource<ITarefa>;
  displayedColumns: string[] = ['titulo', 'descricao', 'dataLimite', 'concluida', 'acoes'];
  criarOuEditar: boolean = false;
  tarefaExcluir: ITarefa | null = null;
  showDeleteModal: boolean = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private tarefaService: TarefaService, private router: Router) {
    this.dataSource = new MatTableDataSource<ITarefa>();
  }

  ngOnInit() {
    this.listarTarefas();
    this.configurarFiltro();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  configurarFiltro() {
    this.dataSource.filterPredicate = (data: ITarefa, filter: string) => {
      const searchStr = filter.toLowerCase();
      return data.titulo.toLowerCase().includes(searchStr) || 
            data.descricao.toLowerCase().includes(searchStr) ||
            new Date(data.dataLimite).toLocaleDateString().includes(searchStr);
    };
  }

  aplicarFiltro(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) 
      this.dataSource.paginator.firstPage();
  }

  listarTarefas() {
    this.tarefaService.listarTarefas()
      .pipe(
        catchError(error => {
          throw error;
        })
      ).subscribe((tarefas: ITarefa[]) => this.retListaTarefas(tarefas));
  }

  retListaTarefas(lstTarefas: ITarefa[]) {
    this.tarefas = lstTarefas;
    this.dataSource.data = lstTarefas;
  }

  editarTarefa(tarefa: ITarefa) {
    this.criarOuEditar = true;
    this.router.navigate(['/tarefas', tarefa.id]);
  }

  cancelarEdicao() {
    this.criarOuEditar = false;
  }

  adicionarTarefa() {
    this.criarOuEditar = true;
    this.router.navigate(['/tarefas/nova']);
  }

  salvarOuEditarTarefa() {
    this.criarOuEditar = false;
    this.listarTarefas();
  }

  confirmarDelete() {
    this.showDeleteModal = false;
    if (this.tarefaExcluir) {
      this.tarefaService.deletarTarefa(this.tarefaExcluir.id!)
        .pipe(
          catchError(error => {
            throw error;
          })
        ).subscribe(() => {
          this.listarTarefas();
        });
    }
  }

  deletarTarefa(tarefa: ITarefa) {
    this.showDeleteModal = true;
    this.tarefaExcluir = tarefa;
  }

}
