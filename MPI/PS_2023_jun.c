#include <mpi.h>
#include <stdio.h>

#define K 6

int main(int argc, char **argv)
{
    int rank, nprocs;

    MPI_File fh;
    MPI_Status status;
    MPI_Offset offset;
    MPI_Datatype filetype;

    int buff[K];

    MPI_Init(&argc, &argv);
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &nprocs);

    // a)
    for (int i = 0; i < K; ++i)
        buff[i] = rank * K + i;

    MPI_File_open(MPI_COMM_WORLD, "output1.dat", MPI_MODE_CREATE | MPI_MODE_WRONLY, MPI_INFO_NULL, &fh);

    offset = rank * K;
    MPI_File_seek(fh, offset * sizeof(int), MPI_SEEK_SET);
    MPI_File_write(fh, buff, K, MPI_INT, &status);

    MPI_File_close(&fh);

    // b)
    int buff2[K];
    MPI_File_open(MPI_COMM_WORLD, "output1.dat", MPI_MODE_RDONLY, MPI_INFO_NULL, &fh);
    MPI_File_read_at(fh, offset * sizeof(int), buff2, K, MPI_INT, &status);
    MPI_File_close(&fh);

    // for debuggg
    // for (int i = 0; i < K; ++i)
    // {
    //     printf("proces %d: buff[%d]: %d\n", rank, i, buff[i]);
    //     printf("proces %d: buff2[%d]: %d\n\n", rank, i, buff2[i]);
    // }

    int isOk = 1;
    for (int i = 0; i < K && isOk; ++i)
        if (buff[i] != buff2[i])
            isOk = 0;

    if (!isOk)
    {
        printf("Nesto ne valja.\n");
        MPI_Finalize();
        return 0;
    }

    // c)
    MPI_File_open(MPI_COMM_WORLD, "output2.dat", MPI_MODE_CREATE | MPI_MODE_WRONLY, MPI_INFO_NULL, &fh);
    MPI_Type_vector(nprocs, K / nprocs, K, MPI_INT, &filetype);
    MPI_Type_commit(&filetype);

    MPI_File_set_view(fh, K / nprocs, MPI_INT, filetype, "native", MPI_INFO_NULL);

    MPI_File_write_all(fh, buff2, K, MPI_INT, &status);

    MPI_File_close(&fh);

    MPI_Finalize();

    return 0;
}