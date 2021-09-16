﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ShopGestor
{
    class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT Articulos.Id, Codigo, Nombre, ARTICULOS.Descripcion, MARCAS.Descripcion Marca, CATEGORIAS.Descripcion Categoria,ImagenUrl, Precio FROM ARTICULOS INNER JOIN MARCAS ON ARTICULOS.IdMarca = MARCAS.Id INNER JOIN CATEGORIAS ON ARTICULOS.IdCategoria = CATEGORIAS.Id";
                comando.Connection = conexion;
                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.id = (int)lector["Id"];
                    aux.codigo = (string)lector["Codigo"];
                    aux.nombre = (string)lector["Nombre"];
                    aux.descripcion = (string)lector["Descripcion"];
                    //aux.marca = (int)lector["IdMarca"];
                    //aux.descripcionMarca = (string)lector["Marca"];
                    aux.marca = new Marca();
                    aux.marca.descripcion= (string)lector["Marca"];

                    //aux.categoria = (int)lector["IdCategoria"];
                    //aux.descripcionCategoria = (string)lector["Categoria"];
                    aux.categoria = new Categoria();
                    aux.categoria.descripcion= (string)lector["Categoria"];

                    aux.Url = (string)lector["imagenUrl"];
                    aux.precio = (decimal)lector["Precio"];
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

        public Articulo buscarArticulo(Articulo articulo, int criterio)
        {
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT A.id, Codigo, Nombre, A.Descripcion AS DART, IdMarca, M.descripcion DMAR, IdCategoria, C.descripcion DCAT, ImagenUrl, Precio " +
                     "FROM articulos A, marcas M, categorias C " +
                    "WHERE (A.IdMarca = M.Id AND A.IdCategoria = c.Id) AND ";
                switch (criterio)
                {
                    case 0:
                        comando.CommandText += "A.id = @criterio";
                        comando.Parameters.AddWithValue("@criterio", articulo.id);
                        break;
                    case 1:
                        comando.CommandText += "Codigo = @criterio";
                        comando.Parameters.AddWithValue("@criterio", articulo.codigo);
                        break;
                    case 2:
                        comando.CommandText += "Nombre = @criterio";
                        comando.Parameters.AddWithValue("@criterio", articulo.nombre);
                        break;
                    case 3:
                        comando.CommandText += "Descripcion = @criterio";
                        comando.Parameters.AddWithValue("@criterio", articulo.descripcion);
                        break;
                    case 4:
                        comando.CommandText += "IdMarca = @criterio";
                        comando.Parameters.AddWithValue("@criterio", articulo.marca.id);
                        break;
                    case 5:
                        comando.CommandText += "IdCategoria = @criterio";
                        comando.Parameters.AddWithValue("@criterio", articulo.categoria.id);
                        break;
                    case 6:
                        comando.CommandText += "Precio = @criterio";
                        comando.Parameters.AddWithValue("@criterio", articulo.precio);
                        break;
                }

                comando.Connection = conexion;
                conexion.Open();
                lector = comando.ExecuteReader();
                if(lector.Read())
                {
                    articulo.id = (int)lector["id"];
                    articulo.codigo = (string)lector["Codigo"];
                    articulo.nombre = (string)lector["Nombre"];
                    articulo.descripcion = (string)lector["DART"];
                    articulo.marca = new Marca();
                    articulo.marca.id = (int)lector["IdMarca"];
                    articulo.marca.descripcion = (string)lector["DMAR"];
                    articulo.categoria = new Categoria();
                    articulo.categoria.id = (int)lector["IdCategoria"];
                    articulo.categoria.descripcion= (string)lector["DCAT"];
                    articulo.Url = (string)lector["imagenUrl"];
                    articulo.precio = (decimal)lector["Precio"];      
                }
                else
                {
                    articulo.id = -1;
                }
                conexion.Close();
                return articulo;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
