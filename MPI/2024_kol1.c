#include <mpi.h>
#include <stdlib.h>
#include <time.h>
#include <stdio.h>

#define N 10

int main(int argc, char** argv)
{
    srand(time(NULL));

    int rank, nprocs;

    MPI_Init(&argc, &argv);
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);  
    MPI_Comm_size(MPI_COMM_WORLD, &nprocs);

    int* buffer = (int*)malloc(N * sizeof(int));

    for (int i = 0; i < N; i++)
        buffer[i] = rand() % 100 * (rank + 1);

    printf("Process %d generated: ", rank);
    for (int i = 0; i < N; i++)
        printf("%d ", buffer[i]);
    printf("\n");

    MPI_File fh;

    MPI_File_open(MPI_COMM_WORLD, "output.dat", MPI_MODE_CREATE | MPI_MODE_WRONLY, MPI_INFO_NULL, &fh);

    MPI_Offset offset = (MPI_Offset)(nprocs - 1 - rank) * (N * sizeof(int));
    MPI_File_seek(fh, offset, MPI_SEEK_SET);
    MPI_File_write(fh, buffer, N, MPI_INT, MPI_STATUS_IGNORE);

    MPI_File_close(&fh);

    MPI_File_open(MPI_COMM_WORLD, "output.dat", MPI_MODE_RDONLY, MPI_INFO_NULL, &fh);

    MPI_File_read_shared(fh, buffer, N, MPI_INT, MPI_STATUS_IGNORE);

    MPI_File_close(&fh);

    printf("Process %d read: ", rank);
    for (int i = 0; i < N; i++)
        printf("%d ", buffer[i]);
    printf("\n");

    free(buffer);
    MPI_Finalize();
}