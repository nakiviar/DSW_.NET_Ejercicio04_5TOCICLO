using Ejercicio04.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ejercicio04.Controllers
{
    public class NegociosController : Controller
    {

        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        //Definimos la lista de insumos
        IEnumerable<Insumo> listado()
        {
            List<Insumo> temporal = new List<Insumo>();

            SqlCommand cmd = new SqlCommand("sp_insumos",cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cn.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Insumo reg = new Insumo
                {
                    codigo = dr.GetInt32(0),
                    descripcion = dr.GetString(1),
                    medida = dr.GetString(2),
                    preCosto = dr.GetDecimal(3),
                    stock = dr.GetInt32(4),
                };
            temporal.Add(reg);

            }
            dr.Close(); cn.Close();
            return temporal;
        }
        
        //Definir la operacion del Create  y el Edit
        String proceso(String cadena, List<SqlParameter> par, int t)
        {
            string mensaje = "";

            SqlCommand cmd = new SqlCommand(cadena, cn);
            cmd.CommandType = CommandType.StoredProcedure;
       
                foreach(SqlParameter x in par)
                {
                    cmd.Parameters.Add(x);
                }
            cn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                if (t == 0)
                {
                    mensaje = "Registro agregado";
                }
                else
                {
                    mensaje = "Registro actualizado";
                }
            }
            catch(SqlException e)
            {
                mensaje = e.Message;
            }
            finally
            {
                cn.Close();
            }
            return mensaje;
        }


        //Vista listado
        public ActionResult Index()
        {
            return View(listado());
        }

        public ActionResult Create()
        {
            return View(new Insumo());
        }

        [HttpPost]public ActionResult Create(Insumo reg)
        {
            List<SqlParameter> lista = new List<SqlParameter>(){
                new SqlParameter("@cod",reg.codigo),
                new SqlParameter("@des",reg.descripcion),
                new SqlParameter("@med",reg.medida),
                new SqlParameter("@pre",reg.preCosto),
                new SqlParameter("@stock",reg.stock)
            };

            ViewBag.mensaje = proceso("sp_agregar",lista,0);
            return View(reg);
        }

        public ActionResult Edit(int id)
        {
            Insumo reg = listado().Where(x => x.codigo == id).FirstOrDefault();
            return View(reg);
        }

        [HttpPost]
        public ActionResult Edit(Insumo reg)
        {
            List<SqlParameter> lista = new List<SqlParameter>(){
                new SqlParameter("@cod",reg.codigo),
                new SqlParameter("@des",reg.descripcion),
                new SqlParameter("@med",reg.medida),
                new SqlParameter("@pre",reg.preCosto),
                new SqlParameter("@stock",reg.stock)
            };

            ViewBag.mensaje = proceso("sp_actualizar", lista, 1);
            return View(reg);
        }


    }
}