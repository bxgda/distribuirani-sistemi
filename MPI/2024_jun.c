#include <mpi.h>
#include <stdio.h>
#include <stdlib.h>

#define BLOCKS 105
#define K 14   /* 1+2+...+14 = 105 */

// Napisati MPI program koji vrši paralelni upis i čitanje binarne datoteke, prema sledećim zahtevima:

// a) Svaki proces upisuje po 105 proizvoljnih celih brojeva u datoteku file1.dat. Upis se vrši upotrebom pojedinačnih
// pokazivača, dok redosled podataka u fajlu ide od podataka poslednjeg do podataka prvog procesa.

// b) Ponovo otvoriti datoteku. Svaki proces vrši čitanje upravo upisanih podataka upotrebom funkcija sa eksplicitnim
// pomerajem.

// c) Upravo pročitane podatke upisati u novu datoteku, na način prikazan na slici (za slučaj od 3 procesa)

int main(int argc, char** argv) 
{
    int rank, nprocs, buf[BLOCKS];
    
    for(int i = 0; i < BLOCKS; i++) 
        buf[i] = i;

    MPI_Init(&argc, &argv);
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &nprocs);

    MPI_File fh;
    MPI_File_open(MPI_COMM_WORLD, "output.dat", MPI_MODE_CREATE | MPI_MODE_WRONLY, MPI_INFO_NULL, &fh);

    MPI_Offset offset = (nprocs - 1 - rank) * BLOCKS * sizeof(int);
    MPI_File_seek(fh, offset, MPI_SEEK_SET);

    MPI_File_write(fh, buf, BLOCKS, MPI_INT, MPI_STATUS_IGNORE);

    MPI_File_close(&fh);
    // MPI_Barrier(MPI_COMM_WORLD); -- MPI_File_close je kolektivna funkcija, pa nije potrebna barijera

    MPI_File_open(MPI_COMM_WORLD, "output.dat", MPI_MODE_RDONLY, MPI_INFO_NULL, &fh);

    MPI_File_read_at(fh, offset, buf, BLOCKS, MPI_INT, MPI_STATUS_IGNORE);

    MPI_File_close(&fh);

    MPI_File_open(MPI_COMM_WORLD, "output2.dat", MPI_MODE_CREATE | MPI_MODE_WRONLY, MPI_INFO_NULL, &fh);

    int block_lengths[K];
    int displacements[K];
    for (int i = 1; i <= K; i++) 
    {
        block_lengths[i-1] = i;
        displacements[i-1] = nprocs * (i * (i - 1) / 2) + rank * i;
    }

    MPI_Datatype filetype;
    MPI_Type_indexed(K, block_lengths, displacements, MPI_INT, &filetype);
    MPI_Type_commit(&filetype);

    MPI_File_set_view(fh, 0, MPI_INT, filetype, "native", MPI_INFO_NULL);
    MPI_File_write_all(fh, buf, BLOCKS, MPI_INT, MPI_STATUS_IGNORE);

    MPI_Type_free(&filetype);
    MPI_File_close(&fh);

    MPI_Finalize();
    return 0;
}