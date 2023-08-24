using ChatMicrosoft.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;

namespace ChatMicrosoft.Services
{
    public class DatabaseService
    {
        private readonly MySqlConnection connection;

        public DatabaseService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            connection = new MySqlConnection(connectionString);
        }

        public async Task InsertFileEmbedding(string fileName, string textFile, float[] fileEmbedding)
        {
            await connection.OpenAsync();
            var query = "INSERT INTO file_embeddings(file_name, text_file, file_embedding, upload_date) VALUES(@FileName, @TextFile, @Embedding, @uploadDate)";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FileName", fileName);
                command.Parameters.AddWithValue("@TextFile", textFile);
                command.Parameters.AddWithValue("@Embedding", JsonConvert.SerializeObject(fileEmbedding));
                command.Parameters.AddWithValue("@uploadDate", DateTime.Now);

                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }


        public async Task<bool> DeleteFileEmbedding(string fileName)
        {
            try
            {
                await connection.OpenAsync();

                var query = "DELETE FROM file_embeddings WHERE file_name = @FileName";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FileName", fileName);
                    await command.ExecuteNonQueryAsync();
                    return true;

                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                connection.Close();

            }
        }

        public async Task<List<FileEmbedding>> GetAllFileEmbeddings()
        {
            List<FileEmbedding> fileEmbeddings = new List<FileEmbedding>();

            try
            {
                await connection.OpenAsync();

                var query = "SELECT file_name, file_embedding, text_file FROM file_embeddings";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string fileName = reader.GetString("file_name");
                        string embeddingJson = reader.GetString("file_embedding");
                        float[] embedding = JsonConvert.DeserializeObject<float[]>(embeddingJson);


                        fileEmbeddings.Add(new FileEmbedding
                        {
                            file_name = fileName,
                            file_embedding = embedding,
                            text_file = reader.GetString("text_file"),
                        });
                    }
                }

                return fileEmbeddings;
            }
            catch (Exception)
            {
                // Gestisci eventuali eccezioni
                throw;
            }
            finally
            {
                connection.Close();
            }
        }



        public async Task<string> GetFileContent(string fileName)
        {
            try
            {
                await connection.OpenAsync();

                var query = "SELECT text_file FROM file_embeddings WHERE file_name = @FileName";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FileName", fileName);
                    var fileContent = await command.ExecuteScalarAsync();

                    return fileContent as string;
                }
            }
            catch (Exception)
            {
                // Gestisci eventuali eccezioni
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<float[]> GetFileEmbedding(string fileName)
        {
            try
            {
                await connection.OpenAsync();

                var query = "SELECT file_embedding FROM file_embeddings WHERE file_name = @FileName";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FileName", fileName);
                    var embeddingJson = await command.ExecuteScalarAsync() as string;

                    if (!string.IsNullOrEmpty(embeddingJson))
                    {
                        var embeddingArray = JsonConvert.DeserializeObject<float[]>(embeddingJson);
                        return embeddingArray;
                    }

                    return null; // Ritorna null se l'embedding non è stato trovato
                }
            }
            catch (Exception)
            {
                // Gestisci eventuali eccezioni
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<List<string>> GetAllFileNames()
        {
            try
            {
                await connection.OpenAsync();

                var query = "SELECT file_name FROM file_embeddings";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        List<string> fileNames = new List<string>();

                        while (await reader.ReadAsync())
                        {
                            string fileName = reader.GetString("file_name");
                            fileNames.Add(fileName);
                        }

                        return fileNames;
                    }
                }
            }
            catch (Exception)
            {
                // Gestisci eventuali eccezioni
                throw;
            }
            finally
            {
                connection.Close();
            }
        }


    }
}