using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select A.Id IdArticulo,A.IdMarca,A.IdCategoria, A.Codigo Codigo, A.Nombre NombreArticulo, A.Descripcion DescArticulo, A.ImagenUrl Imagen,Precio,C.Descripcion Tipo, M.Descripcion Marca from ARTICULOS A, CATEGORIAS C, MARCAS M where C.Id=A.IdCategoria and M.Id=A.IdMarca";
                //comando.CommandText = "select A.Id IdArticulo,Codigo,Nombre,A.Descripcion DescArticulo,Precio,M.Descripcion Marca,M.Id IdMarca, C.Descripcion Aparato, C.Id IdAparato from articulos A, CATEGORIAS C, MARCAS M where C.Id=A.IdCategoria and M.Id=A.IdMarca";
                //comando.CommandText = "select A.Id, A.IdMarca,A.IdCategoria,Codigo,A.Nombre NombrArticulo,A.ImagenUrl, Precio, C.Descripcion Tipo, M.Descripcion Marca from ARTICULOS A,MARCAS M, CATEGORIAS C where c.Id=A.IdCategoria and M.id= A.IdMarca";
                comando.Connection = conexion;

                conexion.Open();
                lector=comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)lector["IdArticulo"];
                    aux.Codigo = (string)lector["Codigo"];
                    aux.Nombre = (string)lector["NombreArticulo"];
                    aux.Descripcion = (string)lector["DescArticulo"];
                    //------------26/8 ARREGLADO------------
                    aux.Precio = (decimal)lector["Precio"];


                    if (!(lector["Imagen"] is DBNull))
                    
                        aux.ImagenUrl = (string)lector["Imagen"];
                    
                    aux.Tipo = new Categoria();
                    aux.Tipo.Descripcion = (string)lector["Tipo"];
                    //aux.Tipo.Id = (int)lector["IdCategoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)lector["IdMarca"];

                        lista.Add(aux);
                    }

                    conexion.Close();   
                    return lista;
                }
                catch (Exception ex)
                {

                throw ex;
                }
            }

            public void agregar(Articulo nuevo)
            {
                AccesoDatos datos = new AccesoDatos();

                try
                {
                    datos.setConsulta("insert into ARTICULOS(Codigo, Nombre,Descripcion, IdMarca,IdCategoria,ImagenUrl,Precio )values(@Codigo,@nombre,@Descripcion,@IdMarca,@IdCategoria,@ImagenUrl,@Precio)");
                    datos.setParametros("Codigo",nuevo.Codigo);
                    datos.setParametros("Nombre",nuevo.Nombre);
                    datos.setParametros("Descripcion",nuevo.Descripcion);
                    datos.setParametros("IdMarca",nuevo.Marca.Id);
                    datos.setParametros("IdCategoria",nuevo.Tipo.Id);
                    datos.setParametros("ImagenUrl",nuevo.ImagenUrl);
                    datos.setParametros("Precio",nuevo.Precio);

                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                datos.cerrarConexion();
                }
            }

            public void modificar(Articulo articulo)
            {
                AccesoDatos datos = new AccesoDatos();
                try
                {
                    datos.setConsulta("update ARTICULOS set Codigo = @Codigo, Nombre=@Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @ImagenUrl, Precio = @Precio where Id=@Id");
                    datos.setParametros("@Codigo",articulo.Codigo);
                    datos.setParametros("@Nombre",articulo.Nombre);
                    datos.setParametros("@Descripcion",articulo.Descripcion);
                    datos.setParametros("@IdMarca",articulo.Marca.Id);
                    datos.setParametros("@IdCategoria",articulo.Tipo.Id);
                    datos.setParametros("@ImagenUrl",articulo.ImagenUrl);
                    datos.setParametros("@Precio",articulo.Precio);
                    datos.setParametros("@Id",articulo.Id);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    datos.cerrarConexion();
                }
            } 
        }
}
