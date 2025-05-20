using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

using BlazorApp1.ViewModels; 
using BlazorApp1.Data.Entities; 
using BlazorApp1.Data; 
using BlazorApp1.Data.Repositories; 
using codegen.Helpers;
using DocumentFormat.OpenXml.Presentation;


namespace BlazorApp1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PulloutsController : ControllerBase
	{
		private readonly AppDB1Context _context;

		public PulloutsController(AppDB1Context context)
		{
			_context = context;
		}

		[HttpGet]
        [Route("List/Page{currPage:int}/PageSize{pageSize:int}")]
        [Route("List")]
		public async Task<IActionResult> GetAllPullout([FromQuery] PulloutSearch searchInfo,int currPage = 1, int pageSize = 10)
		{
			if(!ModelState.IsValid) return BadRequest();

			List<Pullout> pullouts = await _context.Pullouts
						
						.AsNoTracking().ToListAsync();
			if (!String.IsNullOrEmpty(searchInfo.Keyword))
			{
				
				pullouts = pullouts.Where(p => 
									p.PulloutName.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									|| p.PulloutDescription.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									|| p.PulloutDate.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									)
								.ToList();
			}

				//.Where(f => searchInfo.DateFrom == null || f.CreateDate >= searchInfo.DateFrom)
				//.Where(f => searchInfo.DateTo == null || f.CreateDate <= searchInfo.DateTo)
				//.OrderByDescending(s => s.CreateDate).ToList();

				//if (!String.IsNullOrEmpty(searchInfo.SortOrder))
				//{
				//	var sortCurrent = searchInfo.SortOrder.Split("_").Last();
				//	var sortCurrent = searchInfo.SortOrder.Split("_").First();
				//	if (sortCurrent.Equals("DESC"))
				//	{
				//		products.OrderByDescending(a=>a.)
				//	}
				//}

			// Map entity model to view model
			List<PulloutVM> pulloutsVM = new List<PulloutVM>();
			
			foreach(Pullout pullout in pullouts)
			{
				pulloutsVM.Add(new PulloutVM()
				{
					PulloutId = pullout.PulloutId,
					PulloutName = pullout.PulloutName,
					PulloutDescription = pullout.PulloutDescription,
					PulloutDate = pullout.PulloutDate,
					ReceiptImage = pullout.ReceiptImage
				});
			}

			Pagination<PulloutVM> pagination = new Pagination<PulloutVM>(pulloutsVM, currPage, pageSize);

			return Ok(pagination);
		}
		[HttpGet]
		[Route("GetPulloutById/{id:int}")]
		public async Task<IActionResult> GetPullouttById( int id)
		{
			if (id == null || id == 0)
			{
				return NotFound("Not Found");
			}

			var pullout = await _context.Pullouts
								.FirstOrDefaultAsync(m => m.PulloutId == id);

			if (pullout == null)
			{
				return NotFound("Not Found");
			}

			var Pullout = new PulloutVM()
			{
				PulloutId = pullout.PulloutId,
				PulloutName = pullout.PulloutName,
				PulloutDescription = pullout.PulloutDescription,
				PulloutDate = pullout.PulloutDate
			};

			return Ok(Pullout);
		}


		[HttpPost("Add")]
		public async Task<IActionResult> PostPullout(PulloutVM pullout)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest();
            }

			Pullout pulloutToAdd = new Pullout()
			{
				PulloutName = pullout.PulloutName,
				PulloutDescription = pullout.PulloutDescription,
				PulloutDate = pullout.PulloutDate,
				ReceiptImage = pullout.ReceiptImage
			};

            _context.Pullouts.Add(pulloutToAdd);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
				return BadRequest(ex.Message.ToString());
            }

            return Ok();
		}

		[HttpPost("Update")]
		public async Task<IActionResult> PutPullout(PulloutVM pullout)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest();
            }

			Pullout pulloutToUpdate = new Pullout()
			{
				PulloutId = pullout.PulloutId,
				PulloutName = pullout.PulloutName,
				PulloutDescription = pullout.PulloutDescription,
				PulloutDate = pullout.PulloutDate,
				ReceiptImage = pullout.ReceiptImage
			};

            _context.Attach(pulloutToUpdate).State = EntityState.Modified;

			try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
				return BadRequest(ex.Message.ToString());
            }
            return Ok();
		}

		[HttpDelete("{id}/Delete")]
        public async Task<IActionResult> DeletePullout(int id)
		{
			Pullout pulloutToDelete = await _context.Pullouts
							.AsNoTracking()
							.FirstOrDefaultAsync(m => m.PulloutId == id);
			
			if(pulloutToDelete == null)
				return BadRequest("Not Found");
			
			_context.Pullouts.Attach(new Pullout { PulloutId = id }).State = EntityState.Deleted;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
            {
				return BadRequest(ex.Message.ToString());
            }
            return Ok();
		}

		[HttpPost("delete/bulk")]
        public async Task<IActionResult> DeleteBulk([FromBody]List<int> pullouts)
        {
            if (pullouts.Count > 0)
            {
                foreach (int pullout in pullouts)
                {
                    _context.Pullouts.Attach(new Pullout { PulloutId = pullout }).State = EntityState.Deleted;
                }
				try
				{
					await _context.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					return BadRequest(ex.Message.ToString());
				}
            }
            return Ok();
        }

		#region EXPORT TO EXCEL

        [HttpGet("export/report")]
        public async Task<IActionResult> ExportPullout([FromQuery] PulloutSearch searchInfo)
        {
			List<Pullout> pullouts = await _context.Pullouts
						
						.AsNoTracking().ToListAsync();
			if (!String.IsNullOrEmpty(searchInfo.Keyword))
			{
				
				pullouts = pullouts.Where(p => 
									p.PulloutName.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									|| p.PulloutDescription.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									|| p.PulloutDate.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									)
								.ToList();
			}

				//.Where(f => searchInfo.DateFrom == null || f.CreateDate >= searchInfo.DateFrom)
				//.Where(f => searchInfo.DateTo == null || f.CreateDate <= searchInfo.DateTo)
				//.OrderByDescending(s => s.CreateDate).ToList();

				//if (!String.IsNullOrEmpty(searchInfo.SortOrder))
				//{
				//	var sortCurrent = searchInfo.SortOrder.Split("_").Last();
				//	var sortCurrent = searchInfo.SortOrder.Split("_").First();
				//	if (sortCurrent.Equals("DESC"))
				//	{
				//		products.OrderByDescending(a=>a.)
				//	}
				//}

			// Map entity model to view model
			List<PulloutVM> pulloutsVM = new List<PulloutVM>();
			
			foreach(Pullout pullout in pullouts)
			{
				pulloutsVM.Add(new PulloutVM()
				{
					PulloutId = pullout.PulloutId,
					PulloutName = pullout.PulloutName,
					PulloutDescription = pullout.PulloutDescription,
					PulloutDate = pullout.PulloutDate,
					ReceiptImage = pullout.ReceiptImage
				});
			}
 
            DataTable dt = new DataTable("Pullout");
            dt.Columns.Add("PulloutId", typeof(string));
						dt.Columns.Add("PulloutName", typeof(string));
						dt.Columns.Add("PulloutDescription", typeof(string));
						dt.Columns.Add("PulloutDate", typeof(string));
						dt.Columns.Add("ReceiptImage", typeof(string));

            DataRow dr;

            foreach (var item in pulloutsVM)
            {
                dr = dt.NewRow();

                dr[0] = item.PulloutId;
						dr[1] = item.PulloutName;
						dr[2] = item.PulloutDescription;
						dr[3] = item.PulloutDate;
						dr[4] = item.ReceiptImage;

                dt.Rows.Add(dr);
            }

            var exportExcelHelperService = new ExportExcelHelper();

            var bytes = exportExcelHelperService.CreateExcelWorkBook(dt);

            var data = new ExcelData();
            data.File = bytes;

            var result = Convert.ToBase64String(data.File);

            return Ok(result);
        }
        #endregion

	}
}