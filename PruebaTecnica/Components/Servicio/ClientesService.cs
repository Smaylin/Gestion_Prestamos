using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PruebaTecnica.Components.Models;

public class ClientesService
{
    private readonly string _connectionString;

    public ClientesService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<int> InsertarActualizarCliente(Clientes cliente)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_InsertarActualizarCliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    command.Parameters.Add(new SqlParameter("@Id", cliente.Id.HasValue ? (object)cliente.Id.Value : DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Nombres", string.IsNullOrWhiteSpace(cliente.Nombres)
                                ? throw new ArgumentException("Nombres is required")
                                : cliente.Nombres));
                    command.Parameters.Add(new SqlParameter("@Apellidos", cliente.Apellidos ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Identificacion", cliente.Identificacion ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Genero", cliente.Genero ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Es_empleado", cliente.Es_empleado ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Telefono", cliente.Telefono ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Direccion", cliente.Direccion ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Estado", cliente.Estado ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@PerfilEmpleado", cliente.PerfilEmpleado.HasValue ? (object)cliente.PerfilEmpleado.Value : DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Oficial", cliente.Oficial.HasValue ? (object)cliente.Oficial.Value : DBNull.Value));

                    // Abrir conexión
                    await connection.OpenAsync();

                    // Ejecutar el comando y devolver el número de filas afectadas
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar el error (loguear o lanzar la excepción nuevamente)
            throw new Exception("Error al insertar o actualizar el cliente", ex);
        }
    }

    public async Task<Clientes> ConsultarClientePorId(string identificacion)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand("sp_ConsultarCliente", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Identificacion", identificacion);
            connection.Open();

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                if (reader.Read())
                {
                    return new Clientes()
                    {
                        Id = (int)reader["Id"],
                        Nombres = reader["Nombres"].ToString(),
                        Apellidos = reader["Apellidos"].ToString(),
                        Identificacion = reader["Identificacion"].ToString(),
                        Genero = (char)reader["Genero"],
                        Es_empleado = (char)reader["Es_empleado"],
                        Telefono = reader["Telefono"].ToString(),
                        Direccion = reader["Direccion"].ToString(),
                        Estado = (char)reader["Estado"],
                        PerfilEmpleado = (int)reader["PerfilEmpleado"],
                        Oficial = (int)reader["Oficial"]
                    };
                }
            }
        }

        return null;
    }

    public async Task<List<Clientes>> ConsultarClientes()
    {
        List<Clientes> clientes = new List<Clientes>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(); // Abre la conexión de forma asíncrona

            using (SqlCommand command = new SqlCommand("sp_ConsultarCliente", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Ejecuta y obtiene el lector de forma asíncrona
                {
                    while (await reader.ReadAsync()) // Lee los datos de forma asíncrona
                    {
                        Clientes cliente = new Clientes
                        {
                            Id = (int)reader["Id"],
                            Nombres = reader["Nombres"].ToString(),
                            Apellidos = reader["Apellidos"].ToString(),
                            Identificacion = reader["Identificacion"].ToString(),
                            Genero = (char)reader["Genero"],
                            Es_empleado = (char)reader["Es_empleado"],
                            Telefono = reader["Telefono"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            Estado = (char)reader["Estado"],
                            PerfilEmpleado = (int)reader["PerfilEmpleado"],
                            Oficial = (int)reader["Oficial"]
                        };
                        clientes.Add(cliente);
                    }
                } // Aquí el `using` cerrará el SqlDataReader
            } // Aquí el `using` cerrará el SqlCommand
        } // Aquí el `using` cerrará la conexión SQL

        return clientes;
    }
}
