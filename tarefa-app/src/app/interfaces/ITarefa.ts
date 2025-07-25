export interface ITarefa {
    id: string | null;
    titulo: string;
    descricao: string;
    dataLimite: Date;
    concluida: boolean;
}