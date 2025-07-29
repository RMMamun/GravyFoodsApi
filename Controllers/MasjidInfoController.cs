using MasjidApi.DTO;
using MasjidApi.MasjidRepository;
using MasjidApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MasjidApi.Controllers
{
    [ApiController]
    /*[Route("api/[controller]")]*/
    [Route("[controller]")]
    public class MasjidInfoController : ControllerBase
    {
        private readonly IMasjidInfoService _masjidServeice;
        public MasjidInfoController(IMasjidInfoService _service)
        {
            _masjidServeice = _service;
        }

        // GET: MasjidInfo/Details/5
        [HttpGet("{masjidid}")]
        public async Task<ActionResult<MasjidInfoDTO>> GetMasjidInfo(string masjidid)
        {
            try
            {
                if (string.IsNullOrEmpty(masjidid))
                {
                    return NotFound();
                }

                var masjid = await _masjidServeice.GetMasjidInfoById(masjidid);

                //var masjid = await _context.MasjidInfo.FindAsync(masjidid);
                //var masjid = await _context.MasjidInfo.Where(x => x.MasjidID == masjidid).FirstOrDefaultAsync();

                //var masjid = await _context.MasjidInfo.FirstOrDefaultAsync(x => x.MasjidID == masjidid);

                //.Select(x => MasjidDTO(x))
                //.ToListAsync();

                if (masjid == null)
                {
                    return NotFound();
                }

                return masjid;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class MajidsByCoordination
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string? Address { get; set; }

            public int Kilometers { get; set; }

        }


        // GET: MasjidInfo/Details/5
        [HttpPost("GetAllMasjids")]
        public async Task<ActionResult<IEnumerable<MasjidInfo>>> GetAllMasjids(MajidsByCoordination locCo)
        {
            try
            {
                if (locCo == null)
                {
                    return NotFound();
                }

                var masjid = await _masjidServeice.GetAllMasjids(locCo.Latitude, locCo.Longitude, locCo.Address, locCo.Kilometers);

                if (masjid == null)
                {
                    return NotFound();
                }

                return Ok(masjid);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //public MasjidInfoController(MasjidDBContext context)
        //{
        //    _context = context;
        //}

        // GET: MasjidInfo
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<MasjidInfoDTO>>> GetMasjidInfo()
        //{
        //    //try
        //    //{
        //    //    return _context.MasjidInfoDTO != null ?
        //    //                View(await _context.MasjidInfoDTO.ToListAsync()) :
        //    //                Problem("Entity set 'TodoContext.MasjidInfo'  is null.");
        //    //}
        //    //catch(Exception e)
        //    //{
        //    //    return NotFound();
        //    //}

        //    try
        //    {
        //        return await _context.MasjidInfo
        //            .Select(x => MasjidDTO(x))
        //            .ToListAsync();
        //        //.OrderBy(y => y.MasjidID)
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}



        //System generated code
        //public async Task<IActionResult> Details(string masjidid)
        //{
        //    if (masjidid == null || _context.MasjidInfo == null)
        //    {
        //        return NotFound();
        //    }

        //    var masjidInfo = await _context.MasjidInfo
        //        .FirstOrDefaultAsync(m => m.MasjidID == masjidid);
        //    if (masjidInfo == null)
        //    {
        //        return NotFound();
        //    }

        //    return MasjidDTO(masjidInfo);
        //}


        // GET: MasjidInfo/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: MasjidInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //Commented
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("MasjidID,MasjidName,Address,Latitude,Longitude,ContactNumber,City,CountryId,UserId,Email,Website")] MasjidInfo masjidInfo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(masjidInfo);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(masjidInfo);
        //}

        // GET: MasjidInfo/Edit/5
        //public async Task<IActionResult<MasjidInfoDTO>> Edit(string id)
        //{
        //    if (id == null || _context.MasjidInfo == null)
        //    {
        //        return NotFound();
        //    }

        //    var masjidInfo = await _context.MasjidInfo.FindAsync(id);
        //    if (masjidInfo == null)
        //    {
        //        return NotFound();
        //    }
        //    return MasjidDTO(masjidInfo);
        //}

        // POST: MasjidInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        public async Task<ActionResult<MasjidInfoDTO>> Create(MasjidInfoDTO masjidDTO)
        {
            try
            {
                if (masjidDTO == null)
                {
                    return BadRequest();
                }

                if (masjidDTO.MasjidID == "")
                {
                    masjidDTO.MasjidID = Guid.NewGuid().ToString();
                }

                var masjid = await _masjidServeice.Create(masjidDTO);
                if (masjid != null)
                {
                    return CreatedAtAction(nameof(GetMasjidInfo), new { MasjidId = masjid.MasjidID }, masjid);
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //public async Task<ActionResult> UploadImage([FromBody] byte[] imageData)
        //public async Task<IActionResult> UploadImage([FromForm] IFormFile imageData)
        [HttpPost("uploadImage")]
        public async Task<IActionResult> UploadImage(FileTransferModel imageData)
        {
            try
            {
                if (imageData == null)
                {
                    return BadRequest();
                }

                //masjidDTO.MasjidID = Guid.NewGuid().ToString();

                var masjid = await _masjidServeice.SaveImage(imageData);
                if (masjid != false)
                {
                    return Ok(200);
                    //return CreatedAtAction(nameof(GetMasjidInfo), new { MasjidId = masjid.MasjidID }, masjid);
                }
                else
                {
                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost("UpdateMasjidFacilities")]
        public async Task<IActionResult> UpdateMasjidFacilitiesAsunc(MasjidFacilityDTO imageData)
        {
            try
            {
                if (imageData == null)
                {
                    return BadRequest();
                }

                var masjid = await _masjidServeice.UpdateFacilitiesAsync(imageData);
                if (masjid == "OK")
                {
                    return Ok(200);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //[HttpPost]
        //public async Task<IActionResult> UploadImage(IFormFile file)
        //{
        //    if (file.Length > 0)
        //    {
        //        // Save the file to the server
        //        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", file.FileName);
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }

        //        // Return success response
        //        return Ok(new { filePath });
        //    }

        //    return BadRequest();
        //}


        //Original POST function created by schafolding
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("MasjidID,MasjidName,Address,Latitude,Longitude,ContactNumber,City,CountryId,UserId,Email,Website,ImagePath")] MasjidInfo masjidInfo)
        //{
        //    if (id != masjidInfo.MasjidID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(masjidInfo);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MasjidInfoExists(masjidInfo.MasjidID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(masjidInfo);
        //}

        // GET: MasjidInfo/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null || _context.MasjidInfo == null)
        //    {
        //        return NotFound();
        //    }

        //    var masjidInfo = await _context.MasjidInfo
        //        .FirstOrDefaultAsync(m => m.MasjidID == id);
        //    if (masjidInfo == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(masjidInfo);
        //}

        // POST: MasjidInfo/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    if (_context.MasjidInfo == null)
        //    {
        //        return Problem("Entity set 'TodoContext.MasjidInfo'  is null.");
        //    }
        //    var masjidInfo = await _context.MasjidInfo.FindAsync(id);
        //    if (masjidInfo != null)
        //    {
        //        _context.MasjidInfo.Remove(masjidInfo);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


        // PUT: MasjidInfo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMasjidInfo(string id, MasjidInfoDTO masjidInfoDTO)
        {
            try
            {
                var masjid = await _masjidServeice.Update(masjidInfoDTO);

                return null;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
