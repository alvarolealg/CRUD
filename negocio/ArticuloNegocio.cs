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
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

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
                    aux.Tipo.Id = (int)lector["IdCategoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = (string)lector["Marca"];
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
                datos.setParametros("Codigo", nuevo.Codigo);
                datos.setParametros("Nombre", nuevo.Nombre);
                datos.setParametros("Descripcion", nuevo.Descripcion);
                datos.setParametros("IdMarca", nuevo.Marca.Id);
                datos.setParametros("IdCategoria", nuevo.Tipo.Id);
                datos.setParametros("ImagenUrl", nuevo.ImagenUrl);
                datos.setParametros("Precio", nuevo.Precio);

                datos.ejecutarAccion();

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

        public void modificar(Articulo art)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setConsulta("update ARTICULOS set Codigo = @Cod, Nombre=@Nombre, Descripcion = @Desc, IdMarca = @IdMarca, IdCategoria = @IdCat, ImagenUrl = @Img, Precio = @Precio where Id=@Id");
                datos.setParametros("@Cod", art.Codigo);
                datos.setParametros("@Nombre", art.Nombre);
                datos.setParametros("@Desc", art.Descripcion);
                datos.setParametros("@IdMarca", art.Marca.Id);
                datos.setParametros("@IdCat", art.Tipo.Id);
                datos.setParametros("@Img", art.ImagenUrl);
                datos.setParametros("@Precio", art.Precio);
                datos.setParametros("@Id", art.Id);

                datos.ejecutarAccion();

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

        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setConsulta("delete from ARTICULOS where id = @id");
                datos.setParametros("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "select A.Id IdArticulo,A.IdMarca,A.IdCategoria, A.Codigo Codigo, A.Nombre NombreArticulo, A.Descripcion DescArticulo, A.ImagenUrl Imagen,Precio,C.Descripcion Tipo, M.Descripcion Marca from ARTICULOS A, CATEGORIAS C, MARCAS M where C.Id=A.IdCategoria and M.Id=A.IdMarca And ";
               if(campo =="Tipo" ) 
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "C.Descripcion like '" + filtro + "%' ";
                            break; 
                        case "Termina con":
                            consulta += "C.Descripcion like '%" + filtro + "'";
                            break; 
                        case "Contiene":
                            consulta += "C.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {

                    case "Comienza con":
                        consulta += "Nombre like '" + filtro + "%' ";
                        break;
                    case "Termina con":
                        consulta += "Nombre like '%" + filtro + "'";
                        break;
                    case "Contiene":
                        consulta += "Nombre like '%" + filtro + "%'";
                        break;
                    }
                }
                else
                {
                    switch(criterio)
                    {
                    case "Comienza con":
                        consulta += "A.Descripcion like '" + filtro + "%' ";
                        break;
                    case "Termina con":
                        consulta += "A.Descripcion like '%" + filtro + "'";
                        break;
                    case "Contiene":
                        consulta += "A.Descripcion like '%" + filtro + "%'";
                        break;
                    }
                }
               datos.setConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["IdArticulo"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["NombreArticulo"];
                    aux.Descripcion = (string)datos.Lector["DescArticulo"];
                    aux.Precio = (decimal)datos.Lector["Precio"];


                    if (!(datos.Lector["Imagen"] is DBNull))

                        aux.ImagenUrl = (string)datos.Lector["Imagen"];

                    aux.Tipo = new Categoria();
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Tipo.Id = (int)datos.Lector["IdCategoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];

                    lista.Add(aux);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
