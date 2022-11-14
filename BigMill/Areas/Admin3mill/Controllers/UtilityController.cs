using BigMill.Areas.Admin3mill.Models;
using BigMill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigMill.Areas.Admin3mill.Controllers
{
    public class UtilityController : Controller
    {

        /// <summary>
        /// in controller jahate generate kardane view e 10 comment va replye akhir dar dashboarde admin morede estefade gharar migirad
        /// Tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in controller farakhani shode ast
        /// Tavabe (ListPostComment , ListPostCommentReply) az class haye (PostCommentManagement , PostCommentReplyManagement) dar in controller farakhani shode and
        /// </summary>
        /// <returns>
        /// ActionResult : PartialView(LastCommentsReplies)
        /// </returns>
        [ChildActionOnly]
        public ActionResult LastCommentsReplies()
        {
            PostCommentManagement comment = new PostCommentManagement();
            PostCommentReplyManagement commentReply = new PostCommentReplyManagement();
            List<PostCommentModel> list = new List<PostCommentModel>();
            // comment ha be list ezafe mishavand
            list = comment.ListPostComment();
            // reply ha be list ezafe mishavand
            foreach (var item in commentReply.ListPostCommentReply())
            {
                PostCommentModel ListItem = new PostCommentModel();
                ListItem.ID = item.ID;
                ListItem.CreateOnUtc = item.CreateOnUTC;
                ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreateOnUTC);
                ListItem.Display = item.Display;
                ListItem.F_Posts_Id = item.F_PostsComments_Id;
                ListItem.NumberOfDislikes = item.NumberOfDislikes;
                ListItem.NumberOfLikes = item.NumberOfLikes;
                ListItem.NumberOfReply = -1;
                ListItem.Text = item.Text;
                list.Add(ListItem);
            }
            list = list.OrderByDescending(u => u.CreateOnUtc).Take(10).ToList();
            return PartialView(list);
        }

        /// <summary>
        /// jahate generate kardane charte nazarsanjiye faale hal dar dashboarde admin morede estefade gharar migirad
        /// tabe (UserPollHandler) az classe PollQuestionManagement dar in controller farakhani shode ast
        /// </summary>
        /// <returns>
        /// ActionResult : PartialView(ActivePoll)
        /// </returns>
        [ChildActionOnly]
        public ActionResult ActivePoll()
        {
            PollQuestionManagement PQM = new PollQuestionManagement();
            return PartialView(PQM.UserPollHandler());
        }


        /// <summary>
        /// in controller jahate generate kardane tedade mojudiye harkodam az 4 halate statuse darkhast ha dar ghesmate modiriyyate darkhastha dar dashboarde admin morede estefade gharar migirad
        /// tabe (ListAllTicketInboxes) az classe TicketInboxManagement dar in controller farakhani shode ast
        /// </summary>
        /// <returns>
        /// ActionResult : PartialView(ListTicketInbox)
        /// </returns>
        [ChildActionOnly]
        public ActionResult ListTicketInbox()
        {
            TicketInboxManagement TIM = new TicketInboxManagement();
            using(BigMill.Models.Entities db=new Entities())
            {
                // darkhast ha bar asase status guruh bandi shode va tedade aan ha + name har status bazgardande shode ast
                var count = (from c in db.Tickets group c.Status by c.Status into g select new {number=g.Count(),status=g.Key}).ToList();
                List<string[]> TicketStatus = new List<string[]>();
                int AllCount = 0;
                foreach (var q in count) { 
                    TicketStatus.Add(new string[] { q.status, q.number.ToString() });
                    AllCount = AllCount + q.number;
                }
                TicketStatus.Insert(0,new string[] { "همه", AllCount.ToString() });
         
                @ViewBag.TicketStatus = TicketStatus;
            }
            return PartialView(TIM.ListAllTicketInboxes());

        }
	}
}