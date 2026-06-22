#include <mpi.h>
#include <stdlib.h>

#define FILESIZE (1024*1024*10)     // 10 MB

int main(int argc, char** argv)
{
    int rank, nprocs;

    MPI_Init(&argc, &argv);
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);  
    MPI_Comm_size(MPI_COMM_WORLD, &nprocs);

    int bufsize = FILESIZE / nprocs;
    char* buffer = (char*)malloc(bufsize);

    MPI_File fh;

    MPI_File_open(MPI_COMM_WORLD, "input.dat", MPI_MODE_RDONLY, MPI_INFO_NULL, &fh);

    MPI_Offset offset = (MPI_Offset)(nprocs - 1 - rank) * (bufsize);
    MPI_File_seek(fh, offset, MPI_SEEK_SET);
    MPI_File_read(fh, buffer, bufsize, MPI_CHAR, MPI_STATUS_IGNORE);
    
    MPI_File_close(&fh);

    MPI_File_open(MPI_COMM_WORLD, "output.dat", MPI_MODE_CREATE | MPI_MODE_WRONLY, MPI_INFO_NULL, &fh);

    quarter = bufsize / 4;

    MPI_Datatype filetype;
    MPI_Type_vector(4, quarter, quarter * nprocs, MPI_CHAR, &filetype);
    MPI_Type_commit(&filetype);

    MPI_Offset disp = (MPI_Offset)rank * quarter;

    MPI_File_set_view(fh, disp, MPI_CHAR, filetype, "native", MPI_INFO_NULL);

    MPI_File_write_all(fh, buffer, bufsize, MPI_CHAR, MPI_STATUS_IGNORE);

    MPI_File_close(&fh);
    
    free(buffer);
    MPI_Finalize();
}

// primer da se cita i pise iz tekstualnog fajla
// ulaz bi bio: AAAABBBBCCCC
// a korektan izlaz bi bio: CBACBACBACBA

// #include <mpi.h>
// #include <stdlib.h>

// #define FILESIZE 12

// int main(int argc, char** argv)
// {
//     int rank, nprocs;

//     MPI_Init(&argc, &argv);
//     MPI_Comm_rank(MPI_COMM_WORLD, &rank);  
//     MPI_Comm_size(MPI_COMM_WORLD, &nprocs); // nprocs je uvek 3

//     int bufsize = 4; // 12 / 3
//     char* buffer = (char*)malloc(bufsize);

//     MPI_File fh;

//     // 1. Čitanje u obrnutom redosledu (od zadnjeg procesa ka prvom)
//     MPI_File_open(MPI_COMM_WORLD, "input.txt", MPI_MODE_RDONLY, MPI_INFO_NULL, &fh);

//     MPI_Offset offset = (MPI_Offset)(3 - 1 - rank) * 4;
//     MPI_File_seek(fh, offset, MPI_SEEK_SET);
//     MPI_File_read(fh, buffer, bufsize, MPI_CHAR, MPI_STATUS_IGNORE);
    
//     MPI_File_close(&fh);

//     // 2. Vektorski upis sa preplitanjem za 3 procesa
//     MPI_File_open(MPI_COMM_WORLD, "output.txt", MPI_MODE_CREATE | MPI_MODE_WRONLY, MPI_INFO_NULL, &fh);

//     int quarter = 1; // 4 / 4

//     MPI_Datatype filetype;
//     // Broj blokova je i dalje 4 (iz tvog originalnog koda: MPI_Type_vector(4, ...))
//     // Veličina bloka je 1, a razmak između početaka blokova je sada 1 * 3 = 3 bajta
//     MPI_Type_vector(4, 1, 3, MPI_CHAR, &filetype);
//     MPI_Type_commit(&filetype);

//     MPI_Offset disp = (MPI_Offset)rank * 1;

//     MPI_File_set_view(fh, disp, MPI_CHAR, filetype, "native", MPI_INFO_NULL);

//     MPI_File_write_all(fh, buffer, bufsize, MPI_CHAR, MPI_STATUS_IGNORE);

//     MPI_File_close(&fh);
//     MPI_Type_free(&filetype);
    
//     free(buffer);
//     MPI_Finalize();
//     return 0;
// }