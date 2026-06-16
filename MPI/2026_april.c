#include <mpi.h>
#include <stdlib.h>
#define FILESIZE (1024*1024)

int main(int argc, char** argv) {
    int rank, nprocs, bufsize, half;
    char *buf;
    MPI_File fh;
    MPI_Status status;
    MPI_Datatype filetype;
    MPI_Offset offset, disp;

    MPI_Init(&argc, &argv);
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &nprocs);

    bufsize = FILESIZE / nprocs;
    buf = (char*) malloc(bufsize);

    /* --- 1) Citanje iz input.dat obrnutim redom, pojedinacni pokazivac --- */
    MPI_File_open(MPI_COMM_WORLD, "input.dat",
                  MPI_MODE_RDONLY, MPI_INFO_NULL, &fh);

    offset = (MPI_Offset)(nprocs - 1 - rank) * bufsize;
    MPI_File_seek(fh, offset, MPI_SEEK_SET);
    MPI_File_read(fh, buf, bufsize, MPI_BYTE, &status);

    MPI_File_close(&fh);

    /* ... obrada podataka za vizualizaciju ... */

    /* --- 2) Upis u april.dat: 2 polovine, paralelizovano --- */
    MPI_File_open(MPI_COMM_WORLD, "april.dat",
                  MPI_MODE_CREATE | MPI_MODE_WRONLY, MPI_INFO_NULL, &fh);

    half = bufsize / 2;

    /* 2 bloka od po `half` bajtova, razmak = half*nprocs */
    MPI_Type_vector(2, half, half * nprocs, MPI_BYTE, &filetype);
    MPI_Type_commit(&filetype);

    disp = (MPI_Offset)rank * half;
    MPI_File_set_view(fh, disp, MPI_BYTE, filetype, "native", MPI_INFO_NULL);

    /* kolektivni upis = paralelizacija */
    MPI_File_write_all(fh, buf, bufsize, MPI_BYTE, &status);

    MPI_Type_free(&filetype);
    MPI_File_close(&fh);

    free(buf);
    MPI_Finalize();
    return 0;
}