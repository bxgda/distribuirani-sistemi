#include <stdio.h>
#include <stdlib.h>
#include <mpi.h>


#define FILENAME "output.bin"
#define NUM_INTEGERS_PER_PROCESS 10


int main(int argc, char** argv){

    int rank, nprocs;

    MPI_File fh; // file handle
    MPI_Status status;

    // Inicijalizacija MPI okruzenja i hvatanje ranka procesa
    MPI_Init(&argc, &argv);
    MPI_Comm_rank(MPI_COMM_WORLD, &rank); // rank procesa
    MPI_Comm_size(MPI_COMM_WORLD, &nprocs); // broj procesa u comm_world

    //Priprema podatka za upis u fajl - buffer od 10 int vrednsoti (u zavisnosti od ranka procesa)
    int *buf = (int*)malloc(sizeof(int) * NUM_INTEGERS_PER_PROCESS);
    for (int i = 0; i < NUM_INTEGERS_PER_PROCESS; i++){
        buf[i] = rank * NUM_INTEGERS_PER_PROCESS + i;
    }

    if (rank == 0)
        printf("Pripremljeni bufer u svim procesima\n");
    printf("P%d:", rank);
    for (int i =0; i < NUM_INTEGERS_PER_PROCESS; i++){
        printf("%d ", buf[i]);
    }
    printf("\n");
    


    //Zadatak 1
    //Otvaranje fajla u koji treba da upisujemo redom iz buffera svih
    //P0 P1 P2 P3 - svi redom upisuju u fajl svoje buffere - pravimo offset za svaki proces
    int errorFileOpenning = MPI_File_open(MPI_COMM_WORLD, FILENAME, MPI_MODE_CREATE | MPI_MODE_WRONLY, MPI_INFO_NULL, &fh);
    if (errorFileOpenning != MPI_SUCCESS)
        ;//Moze nesto da se uradi;
        

    MPI_Offset offset = (MPI_Offset)rank*NUM_INTEGERS_PER_PROCESS * sizeof(int); // Mora offset da bude u bajtovima

    //Upisujemo podatke u fajl
    MPI_File_write_at(fh, offset, buf, NUM_INTEGERS_PER_PROCESS, MPI_INT, &status);
    MPI_File_close(&fh);
    MPI_Barrier(MPI_COMM_WORLD);


    //Zadatak 2
    //Otvara se fajl isti ali ovaj put za citanje i procesi citaju podatke i upisuju svaki u svoj bufer
    MPI_File_open(MPI_COMM_WORLD, FILENAME, MPI_MODE_RDONLY, MPI_INFO_NULL, &fh);
    MPI_File_read_at(fh, offset, buf, NUM_INTEGERS_PER_PROCESS, MPI_INT, &status);


    if (rank == 0)
        printf("\n\nProcitano iz fajla\n");
    printf("P%d:", rank);
    for (int i =0; i < NUM_INTEGERS_PER_PROCESS; i++){
        printf("%d ", buf[i]);
    }
    printf("\n");

    MPI_File_close(&fh);
    MPI_Barrier(MPI_COMM_WORLD);

    //Zadatak 3
    //Upis u fajl po round robin principu i to po dva elementa se upisuju
    //Proces radi - xx###### (x upisuje, # preskace)
    MPI_File_open(MPI_COMM_WORLD, FILENAME, MPI_MODE_CREATE | MP_MODE_WRONLY, MPI_INFO_NULL, &fh);


    int block_len = 2; // koliko integera po bloku upisuje proces
    int num_blocks = NUM_INTEGERS_PER_PROCESS / block_len; // Koliko ima blokova ukupno od po 2 integera

    MPI_Datatype file_type;
    MPI_Type_vector(num_blocks, block_len, block_len * nprocs, MPI_INT, &file_type);
    MPU_Type_commit(&file_type);

    //Postavljanje view-a na file za svaki od procesa
    MPI_Offset disp = (MPI_Offset)rank * block_len * sizeof(int); // to je zapravo bajt na kom pocinje pogled na fajl

    MPI_File_set_view(fh, disp, MPI_INT, file_type, "native", MPI_INFO_NULL);
    MPI_File_write_all(fh, buf, NUM_INTEGERS_PER_PROCESS, MPI_INT, &status);

    MPI_Type_free(file_type);
    MPI_File_close(&fh);
    MPI_Barrier(MPI_COMM_WORLD);

    //Oslobadjanje memorije i zavrsavanje MPI programa
    free(buf);
    MPI_Finalize();

    return 0;
}


